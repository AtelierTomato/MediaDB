BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Part" (
	"SeriesID"	INTEGER NOT NULL,
	"PartID"	TEXT NOT NULL,
	"LengthTime"	TEXT,
	"LengthWords"	INTEGER,
	"StartTime"	TEXT,
	"EndTime"	TEXT,
	PRIMARY KEY("SeriesID","PartID"),
	FOREIGN KEY("SeriesID") REFERENCES "Series"("ID") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "PartGroupInfo" (
	"SeriesID"	INTEGER NOT NULL,
	"ParentPartID"	TEXT NOT NULL,
	"AverageLengthTime"	TEXT,
	"AverageLengthWords"	INTEGER,
	"MediaType"	TEXT NOT NULL,
	"ReleaseType"	TEXT NOT NULL,
	PRIMARY KEY("SeriesID","ParentPartID"),
	FOREIGN KEY("SeriesID") REFERENCES "Series"("ID") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "PartGroupName" (
	"SeriesID"	INTEGER NOT NULL,
	"ParentPartID"	TEXT NOT NULL,
	"Language"	TEXT NOT NULL,
	"Script"	TEXT NOT NULL,
	"Name"	TEXT NOT NULL,
	PRIMARY KEY("Script","Language","ParentPartID","SeriesID"),
	FOREIGN KEY("SeriesID") REFERENCES "Series"("ID") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "PartName" (
	"SeriesID"	INTEGER NOT NULL,
	"PartID"	TEXT NOT NULL,
	"Language"	TEXT NOT NULL,
	"Script"	TEXT NOT NULL,
	"Name"	TEXT,
	PRIMARY KEY("SeriesID","PartID","Language","Script"),
	FOREIGN KEY("SeriesID") REFERENCES "Series"("ID") ON DELETE CASCADE,
	FOREIGN KEY("SeriesID","PartID") REFERENCES "Part"("SeriesID","PartID") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Series" (
	"ID"	INTEGER NOT NULL UNIQUE,
	"OriginCountries"	TEXT NOT NULL,
	"OriginLanguage"	TEXT,
	"OriginScript"	TEXT,
	"StartTime"	TEXT,
	"EndTime"	TEXT,
	PRIMARY KEY("ID" AUTOINCREMENT),
	CHECK("ID" > 0)
);
CREATE TABLE IF NOT EXISTS "SeriesName" (
	"ID"	INTEGER NOT NULL,
	"Language"	TEXT NOT NULL,
	"Script"	TEXT NOT NULL,
	"Name"	TEXT NOT NULL,
	PRIMARY KEY("ID","Language","Script"),
	FOREIGN KEY("ID") REFERENCES "Series"("ID") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "SeriesParent" (
	"ID"	INTEGER NOT NULL,
	"ParentID"	INTEGER NOT NULL,
	PRIMARY KEY("ID","ParentID"),
	FOREIGN KEY("ID") REFERENCES "Series"("ID") ON DELETE CASCADE,
	FOREIGN KEY("ParentID") REFERENCES "Series"("ID") ON DELETE CASCADE,
	CHECK("ID" <> "ParentID")
);
CREATE TRIGGER "CascadingDeletePartGroupInfo"
-- This trigger exists to enact a sort of conditional foreign key.
AFTER DELETE ON "Part"
FOR EACH ROW
BEGIN
    DELETE FROM "PartGroupInfo"
    WHERE "SeriesID" = OLD."SeriesID" AND "ParentPartID" = OLD."PartID";
END;
CREATE TRIGGER "CascadingDeletePartGroupName"
-- This trigger exists to enact a sort of conditional foreign key.
AFTER DELETE ON "Part"
FOR EACH ROW
BEGIN
    DELETE FROM "PartGroupName"
    WHERE "SeriesID" = OLD."SeriesID" AND "ParentPartID" = OLD."PartID";
END;
CREATE TRIGGER "CheckPartExistsPartGroupInfo" 
-- This trigger exists to enact a sort of conditional foreign key.
BEFORE INSERT ON "PartGroupInfo"
FOR EACH ROW
WHEN NEW.ParentPartID <> ''
BEGIN
    SELECT RAISE(ABORT, 'Foreign key constraint failed: (SeriesID, ParentPartID) pair does not exist in Part')
    WHERE NOT EXISTS (
        SELECT 1 FROM Part 
        WHERE SeriesID = NEW.SeriesID AND PartID = NEW.ParentPartID
    );
END;
CREATE TRIGGER "CheckPartExistsPartGroupName" 
-- This trigger exists to enact a sort of conditional foreign key.
BEFORE INSERT ON "PartGroupName"
FOR EACH ROW
WHEN NEW.ParentPartID <> ''
BEGIN
    SELECT RAISE(ABORT, 'Foreign key constraint failed: (SeriesID, ParentPartID) pair does not exist in Part')
    WHERE NOT EXISTS (
        SELECT 1 FROM Part 
        WHERE SeriesID = NEW.SeriesID AND PartID = NEW.ParentPartID
    );
END;
COMMIT;

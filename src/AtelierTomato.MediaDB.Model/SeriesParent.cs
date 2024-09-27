namespace AtelierTomato.MediaDB.Model
{
	public class SeriesParent
	{
		public ulong ID { get; set; }
		public ulong ParentID { get; set; }
		public SeriesParent(ulong ID, ulong parentID)
		{
			if (ID == parentID)
				throw new ArgumentException("A series cannot have a parent that is itself.", nameof(parentID));

			this.ID = ID;
			ParentID = parentID;
		}
	}
}

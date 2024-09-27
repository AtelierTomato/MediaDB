namespace AtelierTomato.MediaDB.Model
{
	public class Series
	{
		public ulong ID { get; set; }
		public string Name { get; set; }
		public Series(ulong ID, string name)
		{
			this.ID = ID;
			Name = name;
		}
	}
}

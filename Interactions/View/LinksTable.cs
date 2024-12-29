class Links(int id, string link)
{
	public int Id = id;
	public string Link = link;

	public static readonly string Name = "Links";
	public static readonly string IdColumnName = "Id";
	public static readonly string SourceUserIdColumnName = "SourceUserId";
	public static readonly string TargetUserIdColumnName = "TargetUserId";
	public static readonly string LinkColumnName = "Link";
	public static readonly string CreationDateColumnName = "CreationDate";
}

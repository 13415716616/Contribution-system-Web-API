using System;
using System.ComponentModel.DataAnnotations;

public class ManuscriptColumn
{
	[Key]
	public int ManuscriptColumn_ID { get; set; }

	public string ManuscriptColumn_Name { get; set; }

	public string ManuscriptColumn_Dec { get; set; }
}

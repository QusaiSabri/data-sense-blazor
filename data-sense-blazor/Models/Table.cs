﻿namespace data_sense_blazor.Models
{
    public class Table
    {
        public string Name { get; set; }
        public string SchemaName { get; set; }
        public List<Column> Columns { get; set; }
    }
}
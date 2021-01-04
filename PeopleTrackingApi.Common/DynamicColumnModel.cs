namespace PeopleTrackingApi.Common
{
    public class DynamicColumnModel
    {

        public string label { get; set; }
        public string name { get; set; }
        public string index { get; set; }
        public bool hidden { get; set; }
        public bool key { get; set; }
        public bool search { get; set; }
        public bool sortable { get; set; }
        public int width { get; set; }
        public string align { get; set; }
        public int serial { get; set; }
        public string formatter { get; set; }
        public string formatoptions { get; set; }

        public bool IsStringInExle { get; set; }
        public bool IsExcludeInExle { get; set; }


    }

    ////http://www.trirand.com/jqgridwiki/doku.php?id=wiki:predefined_formatter
    public enum JqGridFormatter
    {
        integer,
        number,
        date,
        currency,
        newformat,
        link,
        actions
    }

}

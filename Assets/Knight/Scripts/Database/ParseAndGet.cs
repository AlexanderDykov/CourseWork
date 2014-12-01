using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Assets.Scripts
{
    public class ParseAndGet
    {
        private string header_text = "";
        private string body_text = "";
        private string footer_text = "";
        private Dictionary<string, string> types = new Dictionary<string, string>();
        public ParseAndGet()
        {
            types.Add("Int32", "INTEGER");
            types.Add("String", "TEXT");
            types.Add("DateTime", "DATETIME");
            types.Add("Single", "FLOAT");
            types.Add("Double", "DOUBLE");
            types.Add("Boolean", "BOOLEAN");
        }
        private PropertyInfo[] GetProperties<T>(T item)
        {
            return typeof(T).GetProperties();
        }
        public string CreateInstruction<T>() where T : class
        {
            string foreign_keys = "";
            var f = typeof(T).GetProperties();
            header_text += "CREATE TABLE " + (typeof(T)).Name + " (";
            foreach (var it in f)
            {
                body_text += string.Format("{0} {1} ", it.Name, types[it.PropertyType.Name]);
                var attr = it.GetCustomAttributes(false);
                foreach (var i in attr)
                {
                    if (i is PrimaryKeyField)
                        body_text += ((PrimaryKeyField)i).isPrimaryKey == true ? "PRIMARY KEY NOT NULL " : "";
                    if (i is ForeignKeyField)
                        foreign_keys += ((ForeignKeyField)i).isForeignKey == true ? String.Format("FOREIGN KEY ({0}) REFERENCES {1}({2}) ON DELETE {3} ON UPDATE {4},\n", it.Name, ((ForeignKeyField)i).foreignTableName, ((ForeignKeyField)i).foreignFieldName, "CASCADE", "CASCADE") : "";                    
                }
                body_text += ",\n";
            }
            body_text += foreign_keys;
            footer_text = ");";
            return string.Format("{0}\n{1}{2}", header_text, body_text.Substring(0, body_text.Length - 2), footer_text);
        }
        public string InsertInstruction<T>(T item) where T : class
        {
            header_text = "";
            body_text = "";
            footer_text = "";
            var props = typeof(T).GetProperties();
            header_text += "INSERT INTO " + (typeof(T)).Name + " VALUES (";

            foreach (var it in props)
                switch (converter[(it.GetValue(item, null)).GetType().Name])
                {
                    case Types.Boolean:
                        body_text += string.Format("{0}, ", (it.GetValue(item, null).ToString() == "False") ? 0 : 1);
                        break;

                    case Types.Int32:
                        var attr = it.GetCustomAttributes(false);
                        if (attr != null && attr.Count() > 0)
                        {
                            foreach (var i in attr)
                            {
                                if (i is PrimaryKeyField)
                                    body_text += string.Format("{0}, ", "null");
                                if(i is ForeignKeyField)
                                    body_text += string.Format("{0}, ", it.GetValue(item, null));
                            }
                        }
                        else
                            body_text += string.Format("{0}, ", it.GetValue(item, null));
                        break;

                    case Types.DateTime:
                        body_text += string.Format("'{0}', ", it.GetValue(item, null));
                        break;

                    case Types.String:
                        body_text += string.Format("'{0}', ", it.GetValue(item, null));
                        break;

                    default:
                        body_text += string.Format("{0}, ", it.GetValue(item, null));
                        break;

                }

            footer_text = ");";
            return string.Format("{0}{1}{2}", header_text, body_text.Substring(0, body_text.Length - 2), footer_text); ;
        }
        public IEnumerable<string> InsertCollectionInstruction<T>(IEnumerable<T> collection) where T : class
        {
            List<string> instructions = new List<string>();
            header_text = "";
            body_text = "";
            footer_text = "";
            var props = typeof(T).GetProperties();
            header_text += "INSERT INTO " + (typeof(T)).Name + " VALUES (";

            foreach (var item in collection)
            {
                foreach (var it in props)
                    if ((it.GetValue(item, null)).GetType().Name == Types.String.ToString() || (it.GetValue(item, null)).GetType().Name == Types.DateTime.ToString())
                        body_text += string.Format("'{0}', ", it.GetValue(item, null));
                    else
                        body_text += string.Format("{0}, ", it.GetValue(item, null));
                footer_text = ");";
                instructions.Add(string.Format("{0}{1}{2}", header_text, body_text.Substring(0, body_text.Length - 2), footer_text));
                body_text = "";
            }
            return instructions;
        }
        public string RemoveItemsById<T>(object value)
        {
            if (value.GetType().Name == Types.String.ToString() || value.GetType().Name == Types.DateTime.ToString())
                return "DELETE FROM " + typeof(T).Name + " WHERE Id  = '" + value + "'";
            else
                return "DELETE FROM " + typeof(T).Name + " WHERE Id  = " + value;

        }
        public string UpdateItemsByFieldName<T>(string fieldNameSearch, string valueSearch, string fieldNameToUpdate, string newValue)
        {
            return "UPDATE " + typeof(T).Name + " SET " + fieldNameToUpdate + " = " + newValue + " WHERE " + fieldNameSearch + " = " + valueSearch;
        }
        public string ReadAllInstruction<T>()
        {
            return "SELECT * FROM " + typeof(T).Name;
        }
        public string DropTableInstruction<T>()
        {
            return "DROP TABLE " + typeof(T).Name;
        }
        private Dictionary<string, Types> converter = new Dictionary<string, Types>()
        {
            {"String",Types.String},
            {"DateTime",Types.DateTime},
            {"Double",Types.Double},
            {"Single",Types.Single},
            {"Int32",Types.Int32},
            {"Char",Types.Char},
            {"Boolean",Types.Boolean},
        };
    }
    public enum Types
    {
        String,
        DateTime,
        Double,
        Single,
        Int32,
        Char,
        Boolean
    }
    public class PrimaryKeyField : System.Attribute
    {
        public bool isPrimaryKey = false;
        public PrimaryKeyField(bool mode)
        {
            isPrimaryKey = mode;
        }
    }
    public class ForeignKeyField : System.Attribute
    {
        public bool isForeignKey = false;
        public string foreignTableName { get; set; }
        public string foreignFieldName { get; set; }
        public ForeignKeyField(bool mode, string TabName, string FieldName)
        {
            isForeignKey = mode;
            foreignTableName = TabName;
            foreignFieldName = FieldName;
        }
    }
}

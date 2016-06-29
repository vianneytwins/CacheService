using System;

namespace CachingService
{
    public class MyDataType
    {
        public string Name
        {
            get;
            set;
        }
        public int MyPropertyId
        {
            get;
            set;
        }

        public DateTime LastUpdate
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }


        public override string ToString()
        {
            return String.Format("Name : {0}, Id : {1}, LastUpdate : {2}", Name, Id, LastUpdate);
        }
    }
}


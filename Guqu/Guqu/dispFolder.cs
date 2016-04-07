using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Guqu
{
    public class dispFolder : System.ComponentModel.INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OPC(string propertyName) // OPC = OnPropertyChanged; a helper that raises the PropertyChanged event
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OPC("Name");
                }
            }
        }


        private String _type;
        public String Type { get { return _type; } set { if (_type != value) { _type = value; OPC("Type"); } } }

        private String _size;
        public String Size { get { return _size; } set { if (_size != value) { _size = value; OPC("Size"); } } }

       
        //change to DateTime
        private String _date;
        public String DateModified { get { return _date; } set { if (_date != value) { _date = value; OPC("DateModified"); } } }
        
        
        private String _owners;
        public String Owners { get { return _owners; } set { if (_owners != value) { _owners = value; OPC("Owners"); } } }
        

        private bool _checked;
        public bool Checked { get { return _checked; } set { if (_checked != value) { _checked = value; OPC("Checked"); } } }

        private String fileID;
        public string FileID
        {
            get
            {
                return fileID;
            }

            set
            {
                fileID = value;
            }
        }

        public bool Equals(dispFolder other)
        {
            if (Name.Equals(other.Name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
// BaseModel.cs
//
// Author: Saimel Saez saimelsaez@gmail.com
//
// 7/25/2019
//
//

using System.ComponentModel;

namespace GraphQLQLDemo.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        protected int id;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnProperyChanged(nameof(Id));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnProperyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

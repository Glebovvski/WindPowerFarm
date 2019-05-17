using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindEnergy
{
    public class ViewModel : INotifyPropertyChanged
    {
        WindGenContext context = new WindGenContext();
        private ObservableCollection<WindGenerator> windGenerators;
        public ObservableCollection<WindGenerator> WindGenerators
        {
            get
            {
                return windGenerators;
            }
            set
            {
                windGenerators = value;
                OnPropertyChanged("WindGenerators");
            }
        }


        public ViewModel()
        {
            WindGenerators = new ObservableCollection<WindGenerator>(context.WindGenerators.AsEnumerable());

        }


        public void AddWindGenerator(WindGenerator generator)
        {
            this.WindGenerators.Add(generator);
            context.WindGenerators.Add(generator);
            context.SaveChanges();
            WindGenerators =new ObservableCollection<WindGenerator>(context.WindGenerators.AsEnumerable());
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(name));
        }

        internal void Modify(WindGenerator selected)
        {
            context.Entry(selected).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
    }
}

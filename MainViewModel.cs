using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GlyCombo
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string buttonText;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ButtonText
        {
            get => buttonText;
            set
            {
                if (buttonText != value)
                {
                    buttonText = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
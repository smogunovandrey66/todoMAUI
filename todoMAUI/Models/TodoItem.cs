using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace todoMAUI.Models
{
    public class TodoItem: INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        private bool _isDone = false;
        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (value != _isDone)
                {
                    _isDone = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Decorations));
                }
            }
        }

        public DateTime CreateAt { get; set; }

        private TextDecorations _decoration = TextDecorations.None;
        public TextDecorations Decorations =>
            IsDone ? TextDecorations.Strikethrough : TextDecorations.None;
        //{
        //    get => _decoration;
        //    set
        //    {
        //        if (value != _decoration)
        //        { 
        //            _decoration = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public override string ToString()
        {
            return $"TodoItem[id={Id},title={Title},isDone={IsDone},createAt={CreateAt}]";
        }

    }
}

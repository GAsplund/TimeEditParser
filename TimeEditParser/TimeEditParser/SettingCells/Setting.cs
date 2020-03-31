using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeEditParser.SettingCells
{
    abstract class Setting : ViewCell
    {
        private Label _label { get; set; }
        private View _element { get; set; }

        private Grid _base;


        public string Label
        {
            get
            {
                return _label.Text;
            }
            set
            {
                _label.Text = value;
            }
        }

        internal View Element
        {
            set
            {
                //Remove picker if it exists
                if (_element != null)
                {
                    _base.Children.Remove(_element);
                }

                //Set its value
                _element = value;
                //Add to layout
                _base.Children.Add(_element, 1, 0);

            }
            get
            {
                return _element;
            }
        }

        internal Setting()
        {
            _label = new Label()
            {
                VerticalOptions = LayoutOptions.Center
            };

            _label.SetDynamicResource(Xamarin.Forms.Label.TextColorProperty, "PrimaryTextColor");

            _base = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection() {
                new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Auto) }
                //new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Star) }
            },
                Padding = new Thickness(15, 0)

            };

            _base.Children.Add(_label, 0, 0);

            

            this.View = _base;
        }
    }
}

using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Configurator.ViewModels
{
    public class PipeAngleViewModel : Screen
    {
        #region Private Members
        private double _h1;
        private double _w;
        private double _l1;
        private double _l2;
        private double _t1;
        private double _r1;
        private double _r2;
        private double _r3;
        private double _x1;
        private double _x2;
        private double _x3;
        private double _d1;
        private double _d2;
        private double _d3;

        #endregion

        #region PublicProperties
        public double H1
        {
            get { return _h1; }
            set
            {
                _h1 = value;
                NotifyOfPropertyChange(() => H1);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double W
        {
            get { return _w; }
            set
            {
                _w = value;
                NotifyOfPropertyChange(() => W);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double L1
        {
            get { return _l1; }
            set
            {
                _l1 = value;
                NotifyOfPropertyChange(() => L1);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double L2
        {
            get { return _l2; }
            set
            {
                _l2 = value;
                NotifyOfPropertyChange(() => L2);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double T1
        {
            get { return _t1; }
            set
            {
                _t1 = value;
                NotifyOfPropertyChange(() => T1);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double R1
        {
            get { return _r1; }
            set
            {
                _r1 = value;
                NotifyOfPropertyChange(() => R1);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double R2
        {
            get { return _r2; }
            set
            {
                _r2 = value;
                NotifyOfPropertyChange(() => R2);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double R3
        {
            get { return _r3; }
            set
            {
                _r3 = value;
                NotifyOfPropertyChange(() => R3);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double X1
        {
            get { return _x1; }
            set
            {
                _x1 = value;
                NotifyOfPropertyChange(() => X1);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double X2
        {
            get { return _x2; }
            set
            {
                _x2 = value;
                NotifyOfPropertyChange(() => X2);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double X3
        {
            get { return _x3; }
            set
            {
                _x3 = value;
                NotifyOfPropertyChange(() => X3);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double D1
        {
            get { return _d1; }
            set
            {
                _d1 = value;
                NotifyOfPropertyChange(() => D1);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double D2
        {
            get { return _d2; }
            set
            {
                _d2 = value;
                NotifyOfPropertyChange(() => D2);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }


        public double D3
        {
            get { return _d3; }
            set
            {
                _d3 = value;
                NotifyOfPropertyChange(() => D3);
                NotifyOfPropertyChange(() => CanCreate3DModel);
            }
        }

        #endregion

        public bool CanCreate3DModel
        {
            get
            {
                if (H1 > 0 && W > 0 && L1 > 0 && L2 > 0 && T1 > 0 && R1 > 0 && R2 > 0 && R3 > 0
                    && X1 > 0 && X2 > 0 && X3 > 0 && D1 > 0 && D2 > 0 && D3 > 0)
                    return true;
                else
                    return false;
            }
        }

        public void Create3DModel()
        {
            T1 = 20;
        }

    }
}

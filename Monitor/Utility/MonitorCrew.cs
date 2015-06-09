using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monitor.EntityModels;
using System.ComponentModel;

namespace Monitor.Utility
{
    public class MonitorCrew : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public static string ElderArrived = "прибыл";

        private string _departureDate;
        [DisplayName("Дата отправки")]
        public string DepartureDate
        {
            get { return _departureDate; }
            set
            {
                if (_departureDate != value)
                {
                    _departureDate = value;
                    _OnPropertyChanged(this, "DepartureDate");
                }
            }
        }

        private string _crewNumber;
        [DisplayName("№ ОСП")]
        public string CrewNumber
        {
            get { return _crewNumber; }
            set
            {
                if (_crewNumber != value)
                {
                    _crewNumber = value;
                    _OnPropertyChanged(this, "CrewNumber");
                }
            }
        }

        private string _forcesType;
        [DisplayName("Вид войск")]
        public string ForcesType
        {
            get { return _forcesType; }
            set
            {
                if (_forcesType != value)
                {
                    _forcesType = value;
                    _OnPropertyChanged(this, "ForcesType");
                }
            }
        }

        private string _district;
        [DisplayName("ВО")]
        public string District
        {
            get { return _district; }
            set
            {
                if (_district != value)
                {
                    _district = value;
                    _OnPropertyChanged(this, "District");
                }
            }
        }

        private string _rrStation;
        [DisplayName("Станция")]
        public string RRStation
        {
            get { return _rrStation; }
            set
            {
                if (_rrStation != value)
                {
                    _rrStation = value;
                    _OnPropertyChanged(this, "RRStation");
                }
            }
        }

        private string _unit;
        [DisplayName("В/ч")]
        public string Unit
        {
            get { return _unit; }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    _OnPropertyChanged(this, "Unit");
                }
            }
        }

        private string _duty;
        [DisplayName("Наряд ГШ")]
        public string Duty
        {
            get { return _duty; }
            set
            {
                if (_duty != value)
                {
                    _duty = value;
                    _OnPropertyChanged(this, "Duty");
                }
            }
        }

        private string _overall;
        [DisplayName("Всего")]
        public string Overall
        {
            get { return _overall; }
            set
            {
                if (_overall != value)
                {
                    _overall = value;
                    _OnPropertyChanged(this, "Overall");
                }
            }
        }

        private string _va;
        [DisplayName("ВА")]
        public string VA
        {
            get { return _va; }
            set
            {
                if (_va != value)
                {
                    _va = value;
                    _OnPropertyChanged(this, "VA");
                }
            }
        }

        private string _mtlb;
        [DisplayName("МТЛБ")]
        public string MTLB
        {
            get { return _mtlb; }
            set
            {
                if (_mtlb != value)
                {
                    _mtlb = value;
                    _OnPropertyChanged(this, "MTLB");
                }
            }
        }

        private string _access;
        [DisplayName("Есть допуcк")]
        public string Access
        {
            get { return _access; }
            set
            {
                if (_access != value)
                {
                    _access = value;
                    _OnPropertyChanged(this, "Access");
                }
            }
        }

        private string _elder;
        [DisplayName("Старший")]
        public string Elder
        {
            get { return _elder; }
            set
            {
                if (_elder != value)
                {
                    _elder = value;
                    _OnPropertyChanged(this, "Elder");
                }
            }
        }

        private bool _dressUp;
        [DisplayName("Переодевание")]
        public bool DressUp
        {
            get { return _dressUp; }
            set
            {
                if (_dressUp != value)
                {
                    _dressUp = value;
                    _OnPropertyChanged(this, "DressUp");
                }
            }
        }

        private bool _isSent;
        [DisplayName("Отправка")]
        public bool IsSent
        {
            get { return _isSent; }
            set
            {
                if (_isSent != value)
                {
                    _isSent = value;
                    _OnPropertyChanged(this, "IsSent");
                }
            }
        }

        public MonitorCrew()
        {

        }

        public MonitorCrew(kom kom)
        {
            //_departureDate = (kom.D_OTPR ?? DateTime.MinValue).ToString("dd.MM");
            //_crewNumber = kom.N_KOM;
            //_forcesType = kom.V_VS;
            //_district = kom.V_OKRUG;
            //_rrStation = kom.ST;
            //_unit = kom.V_CH;
            //_duty = kom.NARAD == 0 ? string.Empty : kom.NARAD.ToString();
            //_overall = kom.VSEGO == 0 ? string.Empty : kom.VSEGO.ToString();
            //_va = kom.VA == 0 ? string.Empty : kom.VA.ToString();
            //_mtlb = kom.MTLB == 0 ? string.Empty : kom.MTLB.ToString();
            //_access = kom.DOPUSK == 0 ? string.Empty : kom.DOPUSK.ToString();
            //_elder = !string.IsNullOrEmpty(kom.PREDS) ? ElderArrived : string.Empty;

            //_dressUp = kom.FL_PEREOD != null && kom.FL_PEREOD == 1;
            //_isSent = !string.IsNullOrEmpty(kom.NVESHAT) || (kom.KOLSUHPAY != null && kom.KOLSUHPAY > 0);
            UpdateWithKom(kom);
        }

        private void _OnPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UpdateWithKom(kom kom)
        {
            DepartureDate = (kom.D_OTPR ?? DateTime.MinValue).ToString("dd.MM");
            CrewNumber = kom.N_KOM;
            ForcesType = kom.V_VS;
            District = kom.V_OKRUG;
            RRStation = kom.ST;
            Unit = kom.V_CH;
            Duty = kom.NARAD == 0 ? string.Empty : kom.NARAD.ToString();
            Overall = kom.VSEGO == 0 ? string.Empty : kom.VSEGO.ToString();
            VA = kom.VA == 0 ? string.Empty : kom.VA.ToString();
            MTLB = kom.MTLB == 0 ? string.Empty : kom.MTLB.ToString();
            Access = kom.DOPUSK == 0 ? string.Empty : kom.DOPUSK.ToString();
            Elder = !string.IsNullOrEmpty(kom.PREDS) ? ElderArrived : string.Empty;

            DressUp = kom.FL_PEREOD != null && kom.FL_PEREOD == 1;
            IsSent = !string.IsNullOrEmpty(kom.NVESHAT);
            //IsSent = !string.IsNullOrEmpty(kom.NVESHAT) || (kom.KOLSUHPAY != null && kom.KOLSUHPAY > 0);
        }
    }
}

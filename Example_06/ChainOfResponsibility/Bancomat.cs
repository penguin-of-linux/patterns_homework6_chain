using System.Collections.Generic;

namespace Example_06.ChainOfResponsibility
{
    public enum CurrencyType
    {
        Euro,
        Dollar,
        Ruble
    }

    public class Banknote
    {
        public CurrencyType Currency { get; set; }
        public string Value { get; set; }
    } 

    public class Bancomat
    {
        private readonly BanknoteHandler _handler;

        public Bancomat()
        {
            _handler = new TenRubleHandler(null);
            _handler = new TenDollarHandler(_handler);
            _handler = new TenEuroHandler(_handler);
            _handler = new FiftyRubleHandler(_handler);
            _handler = new FiftyEuroHandler(_handler);
            _handler = new FiftyDollarHandler(_handler);
            _handler = new HundredDollarHandler(_handler);
            _handler = new HundredEuroHandler(_handler);
            _handler = new HundredRubleHandler(_handler);
        }

        public bool Validate(string banknote)
        {
            return _handler.Validate(banknote);
        }

        public bool TryGetCash(int value, CurrencyType currencyType, out List<Banknote> banknotes)
        {
            banknotes = new List<Banknote>();
            return _handler.GetCash(value, currencyType, banknotes);
        }
    }

    public abstract class BanknoteHandler
    {
        private readonly BanknoteHandler _nextHandler;

        protected BanknoteHandler(BanknoteHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public virtual bool Validate(string banknote)
        {
            return _nextHandler != null && _nextHandler.Validate(banknote);
        }

        public virtual bool GetCash(int value, CurrencyType currencyType, List<Banknote> banknotes)
        {
            if (_nextHandler == null)
            {
                if (value == 0) return true;
                banknotes.Clear();
                return false;
            }
            return _nextHandler.GetCash(value, currencyType, banknotes);
        }
    }

    public abstract class RubleHandlerBase : BanknoteHandler
    {
        protected abstract int Value { get; }
        protected CurrencyType CurrencyType => CurrencyType.Ruble;

        public override bool Validate(string banknote)
        {
            if (banknote.Equals($"{Value}₽"))
            {
                return true;
            }
            return base.Validate(banknote);
        }

        public override bool GetCash(int value, CurrencyType currencyType, List<Banknote> banknotes)
        {
            if (currencyType == CurrencyType)
            {
                while (value >= Value)
                {
                    banknotes.Add(new Banknote
                    {
                        Value = $"{Value}₽",
                        Currency = CurrencyType
                    });
                    value -= Value;
                }
            }

            return base.GetCash(value, currencyType, banknotes);
        }

        protected RubleHandlerBase(BanknoteHandler nextHandler) : base(nextHandler)
        {
        }
    }
    
    public class HundredRubleHandler : RubleHandlerBase
    {
        protected override int Value => 100;

        public HundredRubleHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }

    public class FiftyRubleHandler : RubleHandlerBase
    {
        protected override int Value => 50;

        public FiftyRubleHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }

    public class TenRubleHandler : RubleHandlerBase
    {
        protected override int Value => 10;

        public TenRubleHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }

    public abstract class DollarHandlerBase : BanknoteHandler
    {
        protected abstract int Value { get; }
        protected CurrencyType CurrencyType => CurrencyType.Dollar;

        public override bool Validate(string banknote)
        {
            if (banknote.Equals($"{Value}$"))
            {
                return true;
            }
            return base.Validate(banknote);
        }

        public override bool GetCash(int value, CurrencyType currencyType, List<Banknote> banknotes)
        {
            if (currencyType == CurrencyType)
            {
                while (value > Value)
                {
                    banknotes.Add(new Banknote
                    {
                        Value = $"{Value}$",
                        Currency = CurrencyType
                    });
                    value -= Value;
                }
            }

            return base.GetCash(value, currencyType, banknotes);
        }

        protected DollarHandlerBase(BanknoteHandler nextHandler) : base(nextHandler)
        {
        }
    }

    public class HundredDollarHandler : DollarHandlerBase
    {
        protected override int Value => 100;

        public HundredDollarHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }

    public class FiftyDollarHandler : DollarHandlerBase
    {
        protected override int Value => 50;

        public FiftyDollarHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }

    public class TenDollarHandler : DollarHandlerBase
    {
        protected override int Value => 10;

        public TenDollarHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }

    public abstract class EuroHandlerBase : BanknoteHandler
    {
        protected abstract int Value { get; }
        protected CurrencyType CurrencyType => CurrencyType.Euro;

        public override bool Validate(string banknote)
        {
            if (banknote.Equals($"{Value}€"))
            {
                return true;
            }
            return base.Validate(banknote);
        }

        public override bool GetCash(int value, CurrencyType currencyType, List<Banknote> banknotes)
        {
            if (currencyType == CurrencyType)
            {
                while (value > Value)
                {
                    banknotes.Add(new Banknote
                    {
                        Value = $"{Value}€",
                        Currency = CurrencyType
                    });
                    value -= Value;
                }
            }

            return base.GetCash(value, currencyType, banknotes);
        }

        protected EuroHandlerBase(BanknoteHandler nextHandler) : base(nextHandler)
        {
        }
    }

    public class HundredEuroHandler : EuroHandlerBase
    {
        protected override int Value => 100;

        public HundredEuroHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }

    public class FiftyEuroHandler : EuroHandlerBase
    {
        protected override int Value => 50;

        public FiftyEuroHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }

    public class TenEuroHandler : EuroHandlerBase
    {
        protected override int Value => 10;

        public TenEuroHandler(BanknoteHandler nextHandler) : base(nextHandler)
        { }
    }
}

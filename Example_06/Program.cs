using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example_06.ChainOfResponsibility;
using FluentAssertions;

namespace Example_06
{
    class Program
    {
        static void Main(string[] args)
        {
            // получается дублирование, но иначе "Добавить каждому обработчику метод который будет обналичивать переданную сумму" не получится.
            // Я бы запихал GetCash в BanknoteHandler.
            var bankomat = new Bancomat();

            var flag = bankomat.TryGetCash(270, CurrencyType.Ruble, out var banknotes);
            flag.Should().BeTrue();
            banknotes.Should().BeEquivalentTo(new object[]
            {
                new Banknote {Currency = CurrencyType.Ruble, Value = "100₽"},
                new Banknote {Currency = CurrencyType.Ruble, Value = "100₽"},
                new Banknote {Currency = CurrencyType.Ruble, Value = "50₽"},
                new Banknote {Currency = CurrencyType.Ruble, Value = "10₽"},
                new Banknote {Currency = CurrencyType.Ruble, Value = "10₽"},
            });

            flag = bankomat.TryGetCash(271, CurrencyType.Ruble, out banknotes);
            flag.Should().BeFalse();
            banknotes.Should().BeEmpty();
        }
    }
}

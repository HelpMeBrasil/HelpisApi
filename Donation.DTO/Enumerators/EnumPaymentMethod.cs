using System.ComponentModel;

namespace Donation.DTO.Enumerators
{
    public enum EnumPaymentMethod
    {

        [Description("1")] BOLETO = 1,
        [Description("2")] CREDITO = 2,
        [Description("3")] CRIPTOMOEDAS = 3,
        [Description("4")] DEBITO = 4,
        [Description("6")] PIX = 6,
    }
}

using AppService.Domain.Enums;

namespace AppService.Domain.Entities
{
    public class CardCredential : Credential
    {
        public CardType CardType { get; set; }

        public byte[] Data { get; set; }

        public string Pin { get; set; }
    }
}

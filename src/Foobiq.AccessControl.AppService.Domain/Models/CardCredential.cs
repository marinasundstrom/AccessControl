namespace Foobiq.AccessControl.AppService.Domain.Models
{
    public class CardCredential : Credential
    {
        public CardType CardType { get; set; }

        public byte[] Data { get; set; }

        public string Pin { get; set; }
    }
}

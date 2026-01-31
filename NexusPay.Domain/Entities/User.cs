using System;
using System.Collections.Generic;
using System.Text;

namespace NexusPay.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string OAuthProvider { get; set; }
        public string ProviderUserId { get; set; } // unique ID from provider
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastLogin { get; set; }
    }
}

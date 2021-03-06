using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArangoDB.Client;
using ArangoDB.Client.Common.Newtonsoft.Json;

namespace EventSourced.Net.ReadModel.Users.Internal.Documents
{
  public class UserDocument
  {
    public UserDocument() {
      ContactSmsChallenges = new List<UserDocumentContactSmsChallenge>();
      ContactEmailChallenges = new List<UserDocumentContactEmailChallenge>();
      ConfirmedLogins = new List<string>();
    }

    public Guid Id { get; set; }

    [JsonProperty("_key")]
    [DocumentProperty(Identifier = IdentifierType.Key)]
    public string Key => Id.ToString();

    [DocumentProperty(Identifier = IdentifierType.Revision)]
    [JsonProperty("_rev")]
    public string Revision { get; set; }

    public IList<UserDocumentContactSmsChallenge> ContactSmsChallenges { get; }
    public IList<UserDocumentContactEmailChallenge> ContactEmailChallenges { get; }

    public UserDocumentContactChallenge GetContactChallengeByCorrelationId(Guid correlationId) {
      return ContactChallenges.SingleOrDefault(x => x.CorrelationId == correlationId);
    }

    public void AddContactChallenge(UserDocumentContactChallenge challenge) {
      var emailChallenge = challenge as UserDocumentContactEmailChallenge;
      if (emailChallenge != null) {
        AddContactChallenge(emailChallenge);
        return;
      }
      var smsChallenge = challenge as UserDocumentContactSmsChallenge;
      if (smsChallenge != null) {
        AddContactChallenge(smsChallenge);
        return;
      }
      throw new NotSupportedException($"'{challenge.GetType().Name}' is not a supported {typeof(UserDocumentContactChallenge).Name} yet.");
    }

    public void AddContactChallenge(UserDocumentContactSmsChallenge challenge) {
      ContactSmsChallenges.Add(challenge);
    }

    public void AddContactChallenge(UserDocumentContactEmailChallenge challenge) {
      ContactEmailChallenges.Add(challenge);
    }

    [JsonIgnore]
    public IReadOnlyCollection<UserDocumentContactChallenge> ContactChallenges =>
      new ReadOnlyCollection<UserDocumentContactChallenge>(ContactEmailChallenges
        .Cast<UserDocumentContactChallenge>().Union(ContactSmsChallenges).ToArray());

    public string Username { get; set; }
    public IList<string> ConfirmedLogins { get; }

    public void AddConfirmedLogin(Guid correlationId) {
      UserDocumentContactChallenge challenge = GetContactChallengeByCorrelationId(correlationId);
      if (challenge == null) throw new InvalidOperationException(
        $"There is no {typeof(UserDocumentContactChallenge).Name} with correlation id '{correlationId}'.");
      AddConfirmedLogin(challenge.ContactValue);
    }

    public void AddConfirmedLogin(string username) {
      if (!ConfirmedLogins.Any(x => string.Equals(x, username, StringComparison.OrdinalIgnoreCase)))
        ConfirmedLogins.Add(username);
    }
  }
}

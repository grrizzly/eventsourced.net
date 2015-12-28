﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ArangoDB.Client;
using PhoneNumbers;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserIdByLogin : IQuery<Task<Guid?>>
  {
    public string Login { get; }

    public UserIdByLogin(string login) {
      Login = login;
    }
  }

  public class HandleUserIdByLogin : IHandleQuery<UserIdByLogin, Task<Guid?>>
  {
    private IArangoDatabase Db { get; }

    public HandleUserIdByLogin(IArangoDatabase db) {
      Db = db;
    }

    public Task<Guid?> Handle(UserIdByLogin query) {
      Guid? userId = null;
      string normalizedLogin = ContactIdParser.Normalize(query.Login);
      // ReSharper disable ConvertClosureToMethodGroup
      UserView user = Db.Query<UserView>()
        .SingleOrDefault(x => AQL.In(normalizedLogin, x.ConfirmedLogins.Select(y => AQL.Lower(y))));
      // ReSharper restore ConvertClosureToMethodGroup
      if (user != null) userId = user.Id;
      return Task.FromResult(userId);
    }
  }
}

﻿using System;

namespace EventSourced.Net.Domain.Users
{
  public class ContactSmsChallengePrepared : ContactChallengePrepared
  {
    public ContactSmsChallengePrepared(Guid aggregateId, DateTime happenedOn,
      Guid correlationId, ulong phoneNumber, string regionCode, string unformattedPhone,
      ContactChallengePurpose purpose, string stamp, string token, string message)
      : base(aggregateId, happenedOn, correlationId, purpose, stamp, token) {

      PhoneNumber = phoneNumber;
      RegionCode = regionCode;
      UnformattedPhone = unformattedPhone;
      Message = message;
    }

    public ulong PhoneNumber { get; }
    public string RegionCode { get; }
    public string UnformattedPhone { get; }
    public string Message { get; }
  }
}
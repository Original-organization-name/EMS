﻿using EMS.Data.Employee.Enum;

namespace EMS.Contracts.Employee;

public record ContactModel(
    Guid Id,
    ContactType Type,
    string Data, 
    ContractPrivacy Privacy);
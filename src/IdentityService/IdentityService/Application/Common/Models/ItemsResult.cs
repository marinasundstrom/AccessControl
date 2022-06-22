﻿namespace AccessControl.IdentityService.Application.Common.Models;

public record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);

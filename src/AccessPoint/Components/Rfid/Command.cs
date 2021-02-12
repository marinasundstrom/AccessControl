// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Iot.Device.Mfrc522
{
    public enum Command : byte
    {
        Idle = 0x00,
        Authenticate = 0x0E,
        Receive = 0x08,
        Transmit = 0x04,
        Transceive = 0x0C,
        ResetPhase = 0x0F,
        CalculateCRC = 0x03
    }
}

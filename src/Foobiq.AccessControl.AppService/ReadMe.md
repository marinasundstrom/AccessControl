# AccessControl

* Receive commands from client
  * Unlock, Lock

* Send commands to AccessPoint
  * Unlock, Lock
  * Arm, Armed
  
* Listen to events from AccessPoint
  * Door: Open, Closed (Is authorized access?)
  * Lock: Unlocked, Locked (Verify unlock)
  * Alarm: Armed, Disarmed (Verify armament)


Device:
   * DeviceID

AccessList
   * AccessTime
   * Identities/Users
ESK Scoreboard Connector
===

A simple application used to read from a serial port with a scoreboard by [ESK](http://www.esk.com.pl/) and
output data read from there through OSC to a target OSC Server.

Usage:
---

```
ESKScoreboardConnector <COM Port> <Target IP Address> [<Port>]
```

Packets:
---

| OSC Address        | Parameter 1            | Parameter 2           |
| ------------------ | ---------------------- | --------------------- |
| /time              | Minutes (string)       | Seconds (string)      |
| /score             | Score team A  (string) | Score team B (string) |
| /gamePart          | Game part (string)     | N/A                   |

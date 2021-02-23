# Schema

<img src="/docs/raspberry-pi/fritzing/AccessControl_bb.png" />

## MFRC522

| Connector     | Pin (Logical)  |
| ------------- | --------------:|
| SDA           | 8              |
| SCK           | 11             |
| MOSI          | 10             |
| MISO          | 9              |
| GND           | GND            |
| RST           | 22             |
| 3.3v          | 3.3v           |

## Buzzer

| Connector     | Pin (Logical)  |
| ------------- | --------------:|
| +             | 12             |
| GND           | GND            |

GPIO 21 is configured as Hardware PWM.

### Configure Hardware PWM

Edit the /boot/config.txt file and add the dtoverlay line in the file. You need root privileges for this

```
sudo nano /boot/config.txt
```

Paste the following line:

```
dtoverlay=pwm,pin=12,func=4
```

Save the file with ```ctrl + x``` then ```Y``` then ```enter```

Then reboot:

```
sudo reboot
```

## RGB LED (Common Cathode)

| Connector     | Pin (Logical)  |
| ------------- | --------------:|
| R             | 5              |
| GND           | GND            |
| G             | 19             |
| B             | 6              |

R, G, B are configured as Software PWM in software.

## Relay

| Connector     | Pin (Logical)  |
| ------------- | --------------:|
| Input         | 20             |
| + (VCC)       | 3.3v           |
| GND           | GND            |

A Solenoid Lock could be attached as *Normally Open (N.O)*.

## Switch

| Connector     | Pin (Logical)  |
| ------------- | --------------:|
| +             | 3.3v           |
| GND           | 26             |

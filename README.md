<div align="center">

# ESX Deliveries
##### Delivery job hub for fivem. Rent a vehicle and complete various deliveries across town.

<img src="https://i.imgur.com/OqREhqK.jpg" width="90%">

[![npm version](https://img.shields.io/github/release/apoiat/ESX_Deliveries.svg?style=flat)](https://github.com/apoiat/ESX_Deliveries "View this project on npm")  [![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
____

</div>

## INTRODUCTION
**Delivery hub** without a job requirement next to the job center. Quite useful for new players entering your server. Basically, a player will go the symbols (**scooter**, **van**, **truck**), rent a vehicle and make certain deliveries around the map for **payments**. A **safe deposit** is required which is refunded once the player returns the vehicle. **Different props and animations** for each delivery type.

Features:
- 3 different delivery types - **Scooter / Van / Truck**
- Different **props** and animations for every delivery type.
- **Random** delivery route every time.
- **Easy to use** - thorough guide system with **subtitles** and **marks** all along the way.
- **Convars** to customize deliveries count.
- **Convars** to customize payment amount and safe deposits for each delivery type.


### Requirements
* ESX (for the payments)

## Download & Installation

### Manually
- Download https://github.com/apoiat/Esx_Deliveries/releases/download/v1.0/esx_deliveries.zip
- Put it in the `[esx]` directory


## Installation
- Add this in your server.cfg :

```
start esx_deliveries
```
## Configuration
- These following convars are available. If you don't set them, then the default values mentioned below are being used.
```
# Decorcode - you don't need to change this if you don't know what it's usefull for. - default 1450
  setr esx_deliveries_decorcode [int number]

# Minimum deliveries - default 5
  setr esx_deliveries_min [int number]

# Maximum deliveries - default 7
  setr esx_deliveries_max [int number]

# Reward for each delivery made with scooter - default 750
  setr esx_deliveries_reward_scooter [int number]

# Reward for each delivery made with van - default 1000
  setr esx_deliveries_reward_van [int number]

# Reward for each delivery made with truck - default 1450
  setr esx_deliveries_reward_truck [int number] 

# Safe deposit cost for renting the scooter default 4000
  setr esx_deliveries_safe_deposit_scooter [int number]

# Safe deposit cost for renting the van - default 6000
  setr esx_deliveries_safe_deposit_van [int number]

# Safe deposit cost for renting the truck - default 8000
  setr esx_deliveries_safe_deposit_truck [int number]
```



# Legal
### License
ESX_CommunityService - A community service script for fivem servers.

Copyright (C) 2018-2019 Apostolos Iatridis

This program Is free software: you can redistribute it And/Or modify it under the terms Of the GNU General Public License As published by the Free Software Foundation, either version 3 Of the License, Or (at your option) any later version.

This program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty Of MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License For more details.

You should have received a copy Of the GNU General Public License along with this program. If Not, see http://www.gnu.org/licenses/.

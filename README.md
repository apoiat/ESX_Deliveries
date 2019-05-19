<div align="center">
# Esx_Deliveries
##### Delivery job hub for fivem. Rent a vehicle and complete various deliveries across town.

<img src="https://i.imgur.com/bPbBg4J.png" width="690" height="194">


</div>

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

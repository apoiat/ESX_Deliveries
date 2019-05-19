ESX = nil
--local inspect = require('inspect.lua')

TriggerEvent('esx:getSharedObject', function(obj)
    ESX = obj
end)
RegisterServerEvent('esx_deliveries:RemoveCashMoney')
AddEventHandler('esx_deliveries:RemoveCashMoney', function(amount)

    local xPlayer =ESX.GetPlayerFromId(source)

    xPlayer.removeMoney(amount)

end)

RegisterServerEvent('esx_deliveries:AddCashMoney')
AddEventHandler('esx_deliveries:AddCashMoney', function(amount)

    local xPlayer = ESX.GetPlayerFromId(source)

    xPlayer.addMoney(amount)
  
end)

RegisterServerEvent('esx_deliveries:RemoveBankMoney')
AddEventHandler('esx_deliveries:RemoveBankMoney', function(amount)
	
    local xPlayer =ESX.GetPlayerFromId(source)

    xPlayer.removeAccountMoney("bank", amount)
  
end)

RegisterServerEvent('esx_deliveries:AddBankMoney')
AddEventHandler('esx_deliveries:AddBankMoney', function(amount)

     --print("FightClub :: Trying to request $" .. amount .. " from player :: " .. pID.Name)
  
    local xPlayer = ESX.GetPlayerFromId(source)

    xPlayer.addAccountMoney("bank", amount)
    --[[
    if player.getMoney() >= amount then
      TriggerClientEvent("FightClub:BetAccepted", pID.Handle);
      player.removeMoney(amount);
    else
      TriggerClientEvent('esx:showNotification', pID.Handle, "You don't have enough ~r~money~w~.");
    end
    --]]
  
end)


function dump(o)
   if type(o) == 'table' then
      local s = '{ '
      for k,v in pairs(o) do
         if type(k) ~= 'number' then k = '"'..k..'"' end
         s = s .. '['..k..'] = ' .. dump(v) .. ','
      end
      return s .. '} '
   else
      return tostring(o)
   end
end

--[[
RegisterServerEvent('MailDelivery:VanRented')
AddEventHandler('MailDelivery:VanRented', function(amount)
    local _source = source

    local player = ESX.GetPlayerFromId(_source)
    player.removeMoney(tonumber(amount))
end)
--]]
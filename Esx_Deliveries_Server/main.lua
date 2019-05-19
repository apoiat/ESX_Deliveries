ESX = nil

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
 
    local xPlayer = ESX.GetPlayerFromId(source)

    xPlayer.addAccountMoney("bank", amount)

  
end)


resource_manifest_version '44febabe-d386-4d18-afbe-5e627f4af937'

description 'ESX Deliveries'

version '1.0'

client_script "Esx_Deliveries_Client.net.dll"

server_script {
	"server/main.lua"
}
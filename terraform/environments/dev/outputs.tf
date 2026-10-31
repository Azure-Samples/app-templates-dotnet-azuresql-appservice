/* This takes the networking module's vnet_id output and makes it available to 
 other modules that call this module." */

output "vnet_id" {
  description = "ID of the Dev virtual network."
  value       = module.networking.vnet_id
}


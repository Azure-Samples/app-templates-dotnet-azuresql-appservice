output "log_analytics_workspace_id" {
  description = "Azure resource ID of the shared Log Analytics workspace."
  value       = azurerm_log_analytics_workspace.shared.id
}
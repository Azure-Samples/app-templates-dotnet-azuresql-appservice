variable "subscription_id" {
  description = "Dev subscription ID."
  type        = string
}

variable "shared_subscription_id" {
  description = "Subscription containing shared monitoring."
  type        = string
}

variable "tenant_id" {
  description = "Entra ID tenant ID."
  type        = string
}

variable "sql_admin_name" {
  description = "Display name of the SQL Entra administrator."
  type        = string
}

variable "sql_admin_object_id" {
  description = "Object ID of the SQL Entra administrator."
  type        = string
}
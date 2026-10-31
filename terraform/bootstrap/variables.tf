variable "storage_account_name" {
  description = "Globally unique name for Terraform state storage."
  type        = string
}

variable "allowed_public_ip" {
  description = "Public IPv4 address allowed through the storage firewall."
  type        = string
}

variable "subscription_id" {
  description = "Subscription containing Terraform state storage."
  type        = string
}

variable "tenant_id" {
  description = "Entra ID tenant containing the subscription."
  type        = string
}

variable "state_admin_object_id" {
  description = "Object ID of the identity granted state container access."
  type        = string
}
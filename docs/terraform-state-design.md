# Terraform State Design

## Where state lives

The storage account `stcirrustfstatepatty01` is in `rg-cirrus-tfstate`, in the Cirrus-Labs subscription.

Bootstrap: State is stored locally in terraform.tfstate, with an encrypted backup.
Dev: State is stored in Azure, in the dev container as dev.tfstate.
Shared: The container exists, but remote state isn’t configured yet.
Prod: The container exists, but remote state isn’t configured yet.

Bootstrap manages the state storage. Dev will manage the
application resources in sub-cirrus-dev.

Keep the state storage when deleting application resources.

## Security

- Use Entra ID authentication; storage keys are disabled.
- My account has Storage Blob Data Contributor on the Dev container.
- Anonymous access is disabled.
- The firewall allows only my configured public IP.
- HTTPS is required, with minimum TLS 1.2.
- Versioning and 30-day soft deletion are enabled.
- Terraform prevent_destroy guards against deleting the storage account.

The public IP rule is temporary.

State can contain secrets. Keep state files, backups and
local terraform.tfvars out of Git. Commit the lock files.

## Checks completed

- Dev connected to remote state.
- Locking prevented a second Terraform operation.
- My current login was denied blob access to Prod.
- The encrypted bootstrap backup passed its archive test.

Recovery from blob versions and soft deletion was not tested.
The backup’s OneDrive sync still needs confirmation.

## Later work

Configure separate pipeline identities using OIDC and access
to their own containers. Repeat the access tests with those identities.

Move appropriate local input values into GitHub Actions variables
or secrets, passed to Terraform through TF_VAR_*.
Plan the runner’s network access separately.

Refresh the encrypted backup whenever bootstrap changes.

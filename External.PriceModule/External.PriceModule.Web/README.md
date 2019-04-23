# Expansion of the functionality of the price module.

A new field is added to the entity "Price" and the repository is redefined.

# Problems and solutions:

* Migrations

The migration context may become obsolete. In particular, if the state of the database has changed since the generation of the existing migration. Because of this, there will be problems with migration and errors in the integration of the module.
You must recreate the migration or add a new one in order to get the current state of the models. It is also necessary to remove changes to entities that have already been made. Only what we did in the extension should remain.

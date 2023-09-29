# AppSSO Starter Java

This repository provides an example application used to set up an authentication mechanism with AppSSO.

## Getting Started

1. Discover your `AppSSO` service offerings

   ```bash
   tanzu services classes list

   NAME                  DESCRIPTION
   kafka-unmanaged       Kafka by Bitnami
   mongodb-unmanaged     MongoDB by Bitnami
   mysql-unmanaged       MySQL by Bitnami
   postgresql-unmanaged  PostgreSQL by Bitnami
   rabbitmq-unmanaged    RabbitMQ by Bitnami
   redis-unmanaged       Redis by Bitnami
   test-login            Login by AppSSO - user:password - UNSAFE FOR PRODUCTION!
   ```

   If there isn't one,

   - ask your service operator `OR`
   - [create your own `ClusterUnsafeTestLogin`](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.6/tap/app-sso-reference-api-clusterunsafetestlogin.html)

1. Select the service offering you'd like to connect

1. Discover the parameter schema for the desired service
   ```
   tanzu service class get test-login
   ```

1. Claim an instance from the offering

   ```bash
   tanzu services class-claims create blazor-on-tap-sso \
     --class test-login \
     --parameter workloadRef.name=blazor-on-tap \
     --parameter authorizationGrantTypes='["client_credentials", "authorization_code"]' \
     --parameter redirectPaths='["/authentication/login"]' \
     --parameter requireUserConsent=false
   ```

1. tanzu apps workload create blazor-on-tap -f Server/config/workload.yaml

1. Apply your workload

   ```bash
   kubectl apply -f config
   ```

For reference, see
[AppSSO's documentation](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.6/tap/app-sso-about.html)

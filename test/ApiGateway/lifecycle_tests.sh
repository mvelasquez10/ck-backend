#!/bin/bash

# Default to direct mode if not specified
MODE=${1:-direct} # direct or proxy

# Hosts configuration
PROXY_HOST="http://localhost:8080"
USERS_HOST="http://localhost:8084"
POSTS_HOST="http://localhost:8083"
LANGUAGES_HOST="http://localhost:8082"

get_host() {
    service=$1
    if [ "$MODE" == "proxy" ]; then
        echo "$PROXY_HOST/$service"
    else
        case $service in
            Auth|User) echo "$USERS_HOST" ;;
            Post) echo "$POSTS_HOST" ;;
            Language) echo "$LANGUAGES_HOST" ;;
        esac
    fi
}

# 1. Authenticate admin user
echo "Authenticating admin..."
AUTH_RESPONSE=$(curl -s -X POST -H "Content-Type: application/json" -d '{"Email":"admin@ck.com","Password":"admin"}' "$(get_host Auth)/auth")
ADMIN_TOKEN=$(echo "$AUTH_RESPONSE" | jq -r '.token')

if [ -z "$ADMIN_TOKEN" ] || [ "$ADMIN_TOKEN" == "null" ]; then
    echo "Failed to authenticate admin."
    exit 1
fi

echo "Admin authenticated."

# 2. User lifecycle
echo "Testing User lifecycle..."
USER_EMAIL="lifecycle.$RANDOM@user.com"
# Create
CREATE_USER_RESPONSE=$(curl -s -X POST -H "Content-Type: application/json" -H "Authorization: Bearer $ADMIN_TOKEN" -d "{\"Name\":\"Lifecycle\",\"Surname\":\"User\",\"Email\":\"$USER_EMAIL\",\"Password\":\"password\"}" "$(get_host User)/user")
USER_ID=$(echo "$CREATE_USER_RESPONSE" | jq -r '.id')
# Read
curl -s -X GET -H "Authorization: Bearer $ADMIN_TOKEN" "$(get_host User)/user/$USER_ID" | jq .
# Update
curl -s -X PUT -H "Content-Type: application/json" -H "Authorization: Bearer $ADMIN_TOKEN" -d '{"Name":"Updated"}' "$(get_host User)/user/$USER_ID"
# Delete
curl -s -X DELETE -H "Authorization: Bearer $ADMIN_TOKEN" "$(get_host User)/user/$USER_ID"
echo "User lifecycle test complete."

# 3. Language lifecycle
echo "Testing Language lifecycle..."
# Create
CREATE_LANG_RESPONSE=$(curl -s -X POST -H "Content-Type: application/json" -H "Authorization: Bearer $ADMIN_TOKEN" -d '{"Name":"TestLang"}' "$(get_host Language)/language")
LANG_ID=$(echo "$CREATE_LANG_RESPONSE" | jq -r '.id')
# Read
curl -s -X GET "$(get_host Language)/language/$LANG_ID" | jq .
# Update
curl -s -X PUT -H "Content-Type: application/json" -H "Authorization: Bearer $ADMIN_TOKEN" -d '{"Name":"UpdatedLang"}' "$(get_host Language)/language/$LANG_ID"
# Delete
curl -s -X DELETE -H "Authorization: Bearer $ADMIN_TOKEN" "$(get_host Language)/language/$LANG_ID"
echo "Language lifecycle test complete."

# 4. Post lifecycle
echo "Testing Post lifecycle..."
# Create
CREATE_POST_RESPONSE=$(curl -s -X POST -H "Content-Type: application/json" -H "Authorization: Bearer $ADMIN_TOKEN" -d '{"Title":"Test Post","Description":"A test post.","Language":1,"Snippet":"Test snippet"}' "$(get_host Post)/post")
POST_ID=$(echo "$CREATE_POST_RESPONSE" | jq -r '.id')
# Read
curl -s -X GET "$(get_host Post)/post/$POST_ID" | jq .
# Update
curl -s -X PUT -H "Content-Type: application/json" -H "Authorization: Bearer $ADMIN_TOKEN" -d '{"Title":"Updated Post"}' "$(get_host Post)/post/$POST_ID"
# Delete
curl -s -X DELETE -H "Authorization: Bearer $ADMIN_TOKEN" "$(get_host Post)/post/$POST_ID"
echo "Post lifecycle test complete."

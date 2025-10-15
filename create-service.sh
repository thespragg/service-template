#!/usr/bin/env bash
set -e

read -p "Project name (PascalCase): " APP_NAME

if [[ -z "$APP_NAME" ]]; then
  echo "Project name required."
  exit 1
fi

APP_NAME_LOWER=$(echo "$APP_NAME" | tr '[:upper:]' '[:lower:]')

TMP_DIR=$(mktemp -d)

curl -L -o "$TMP_DIR/template.zip" "https://github.com/thespragg/service-template/archive/refs/heads/main.zip"

unzip -q "$TMP_DIR/template.zip" -d "$TMP_DIR"

TEMPLATE_DIR=$(find "$TMP_DIR" -maxdepth 1 -type d -name "*service-template-main*")

cp -R "$TEMPLATE_DIR" "$APP_NAME"

find "$APP_NAME" -type f -exec sed -i "s/{{APP_NAME}}/$APP_NAME/g" {} +
find "$APP_NAME" -type f -exec sed -i "s/{{APP_NAME_LOWER}}/$APP_NAME_LOWER/g" {} +

echo "Project '$APP_NAME' created successfully at $(pwd)/$APP_NAME"

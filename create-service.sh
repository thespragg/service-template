#!/usr/bin/env bash
set -e

read -p "Project name: " INPUT_NAME
if [[ -z "$INPUT_NAME" ]]; then
  echo "Project name required."
  exit 1
fi

CLEAN_NAME=$(echo "$INPUT_NAME" | tr -cd '[:alnum:]_-' )

APP_NAME_LOWER=$(echo "$CLEAN_NAME" \
  | sed -E 's/_/-/g' \
  | sed -E 's/([a-z0-9])([A-Z])/\1-\L\2/g' \
  | tr '[:upper:]' '[:lower:]')

APP_NAME=$(echo "$CLEAN_NAME" \
  | sed -E 's/[-_]+/ /g' \
  | sed -E 's/(^| )([a-z])/\U\2/g' \
  | tr -d ' ')

TMP_DIR=$(mktemp -d)

curl -L -o "$TMP_DIR/template.zip" "https://github.com/yourusername/yourtemplate/archive/refs/heads/main.zip"

unzip -q "$TMP_DIR/template.zip" -d "$TMP_DIR"

TEMPLATE_DIR=$(find "$TMP_DIR" -maxdepth 1 -type d -name "*yourtemplate-main*")

cp -R "$TEMPLATE_DIR/template/." "$APP_NAME"

find "$APP_NAME" -type f -exec sed -i "s/{{APP_NAME}}/$APP_NAME/g" {} +
find "$APP_NAME" -type f -exec sed -i "s/{{APP_NAME_LOWER}}/$APP_NAME_LOWER/g" {} +

echo "Project '$APP_NAME' created successfully at $(pwd)/$APP_NAME"

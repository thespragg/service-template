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

curl -L -o "$TMP_DIR/template.zip" "https://github.com/thespragg/service-template/archive/refs/heads/main.zip"

unzip -q "$TMP_DIR/template.zip" -d "$TMP_DIR"

TEMPLATE_DIR=$(find "$TMP_DIR" -maxdepth 1 -type d -name "*service-template-main*")

cp -R "$TEMPLATE_DIR/template/." "$APP_NAME"

EXTENSIONS=("*.vue" "*.js" "*.ts" "*.json" "*.css" "*.scss" "*.html" \
            "*.cs" "*.csproj" "*.sln" "*.config" "*.xml" \
            "*.md" "*.txt" "*.yml" "*.yaml" "*.sh")

FIND_EXPR=""
for ext in "${EXTENSIONS[@]}"; do
  FIND_EXPR="$FIND_EXPR -o -name \"$ext\""
done
FIND_EXPR="${FIND_EXPR# -o }"

for PLACEHOLDER in APP_NAME APP_NAME_LOWER; do
  VALUE="${!PLACEHOLDER}"
  eval "find \"$APP_NAME\" -type f \( $FIND_EXPR \) -exec sed -i '' \"s/{{${PLACEHOLDER}}}/$VALUE/g\" {} +"
done


echo "Project '$APP_NAME' created successfully at $(pwd)/$APP_NAME"

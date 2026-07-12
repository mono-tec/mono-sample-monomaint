#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPOSITORY_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

PACKAGE_ROOT="${SCRIPT_DIR}/package"
PUBLISH_DIR="${REPOSITORY_ROOT}/publish/linux"
APPLICATION_DIR="${PACKAGE_ROOT}/opt/monomaint"
OUTPUT_DIR="${SCRIPT_DIR}/output"

PACKAGE_NAME="monomaint-sample"
PACKAGE_VERSION="0.1.0"
PACKAGE_ARCHITECTURE="amd64"
PACKAGE_FILE="${OUTPUT_DIR}/${PACKAGE_NAME}_${PACKAGE_VERSION}_${PACKAGE_ARCHITECTURE}.deb"

echo "Cleaning package application directory..."
rm -rf "${APPLICATION_DIR}"
mkdir -p "${APPLICATION_DIR}"
mkdir -p "${OUTPUT_DIR}"

echo "Copying publish output..."
cp -a "${PUBLISH_DIR}/." "${APPLICATION_DIR}/"

echo "Setting maintainer script permissions..."
find "${PACKAGE_ROOT}/DEBIAN" \
  -maxdepth 1 \
  -type f \
  ! -name control \
  -exec chmod 0755 {} \;

echo "Building Debian package..."
dpkg-deb --build \
  --root-owner-group \
  "${PACKAGE_ROOT}" \
  "${PACKAGE_FILE}"

echo "Created package:"
echo "${PACKAGE_FILE}"
#!/usr/bin/env bash

set -euo pipefail

############################################################
# MonoMaint Debian Package Build Script
#
# GitHub ActionsなどのLinux環境上で実行し、
# Linux向けpublish成果物からDebianパッケージを作成します。
#
# パッケージのバージョンは、Hostプロジェクトの
# MonoMaint.Host.Sample.csproj に定義された Version を正とします。
#
# 処理概要
#   1. 入力ファイルを確認
#   2. プロジェクトファイルからVersionを取得
#   3. パッケージ作業領域を初期化
#   4. DEBIAN/controlを動的生成
#   5. publish成果物をパッケージ領域へコピー
#   6. Linux用ファイルの改行・BOMを正規化
#   7. メンテナンススクリプトへ実行権限を付与
#   8. dpkg-debで.debパッケージを生成
############################################################


############################################################
# パス設定
############################################################

# build-deb.shが配置されているディレクトリ
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# リポジトリのルートディレクトリ
REPOSITORY_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

# バージョン取得元となるHostプロジェクト
PROJECT_FILE="${REPOSITORY_ROOT}/src/Hosts/MonoMaint.Host.Sample/MonoMaint.Host.Sample.csproj"

# Linux向けpublish成果物
PUBLISH_DIR="${REPOSITORY_ROOT}/publish/linux"

# Debianパッケージの作業ディレクトリ
PACKAGE_ROOT="${SCRIPT_DIR}/package"

# アプリケーションのパッケージ内配置先
APPLICATION_DIR="${PACKAGE_ROOT}/opt/monomaint"

# Debian制御ファイルの配置先
DEBIAN_DIR="${PACKAGE_ROOT}/DEBIAN"
CONTROL_FILE="${DEBIAN_DIR}/control"

# Debianメンテナンススクリプト
POSTINST_FILE="${DEBIAN_DIR}/postinst"
PRERM_FILE="${DEBIAN_DIR}/prerm"
POSTRM_FILE="${DEBIAN_DIR}/postrm"

# systemd Unitファイル
SERVICE_FILE="${PACKAGE_ROOT}/lib/systemd/system/monomaint-sample.service"

# 生成した.debの出力先
OUTPUT_DIR="${SCRIPT_DIR}/output"


############################################################
# パッケージ情報
############################################################

PACKAGE_NAME="monomaint-sample"
PACKAGE_ARCHITECTURE="amd64"
PACKAGE_SECTION="web"
PACKAGE_MAINTAINER="mono-tec"
PACKAGE_DEPENDS="aspnetcore-runtime-10.0"
PACKAGE_DESCRIPTION="MonoMaint Sample application"


############################################################
# 入力ファイル確認
############################################################

if [[ ! -f "${PROJECT_FILE}" ]]; then
    echo "ERROR: Project file was not found."
    echo "Expected: ${PROJECT_FILE}"
    exit 1
fi

if [[ ! -d "${PUBLISH_DIR}" ]]; then
    echo "ERROR: Linux publish directory was not found."
    echo "Expected: ${PUBLISH_DIR}"
    exit 1
fi

if [[ ! -f "${POSTINST_FILE}" ]]; then
    echo "ERROR: postinst file was not found."
    echo "Expected: ${POSTINST_FILE}"
    exit 1
fi

if [[ ! -f "${PRERM_FILE}" ]]; then
    echo "ERROR: prerm file was not found."
    echo "Expected: ${PRERM_FILE}"
    exit 1
fi

if [[ ! -f "${POSTRM_FILE}" ]]; then
    echo "ERROR: postrm file was not found."
    echo "Expected: ${POSTRM_FILE}"
    exit 1
fi

if [[ ! -f "${SERVICE_FILE}" ]]; then
    echo "ERROR: systemd Unit file was not found."
    echo "Expected: ${SERVICE_FILE}"
    exit 1
fi


############################################################
# プロジェクトファイルからVersionを取得
############################################################

echo "Reading package version from project file..."

PACKAGE_VERSION="$(
    dotnet msbuild "${PROJECT_FILE}" \
        -nologo \
        -getProperty:Version
)"

# 空白・改行を除去
PACKAGE_VERSION="$(printf '%s' "${PACKAGE_VERSION}" | tr -d '\r\n[:space:]')"

if [[ -z "${PACKAGE_VERSION}" ]]; then
    echo "ERROR: Version could not be read from project file."
    exit 1
fi

echo "Package version: ${PACKAGE_VERSION}"


############################################################
# パッケージ作業領域を初期化
############################################################

echo "Cleaning package build directories..."

# 前回コピーしたpublish成果物を削除
rm -rf "${APPLICATION_DIR}"

# 前回生成したcontrolを削除
rm -f "${CONTROL_FILE}"

# 前回生成した.debを削除
rm -rf "${OUTPUT_DIR}"

mkdir -p "${APPLICATION_DIR}"
mkdir -p "${DEBIAN_DIR}"
mkdir -p "${OUTPUT_DIR}"


############################################################
# DEBIAN/controlを動的生成
############################################################

echo "Generating Debian control file..."

cat > "${CONTROL_FILE}" <<EOF
Package: ${PACKAGE_NAME}
Version: ${PACKAGE_VERSION}
Section: ${PACKAGE_SECTION}
Priority: optional
Architecture: ${PACKAGE_ARCHITECTURE}
Maintainer: ${PACKAGE_MAINTAINER}
Depends: ${PACKAGE_DEPENDS}
Description: ${PACKAGE_DESCRIPTION}
 Plugin-based maintenance platform sample application.
EOF

chmod 0644 "${CONTROL_FILE}"


############################################################
# publish成果物をパッケージへコピー
############################################################

echo "Copying publish output..."

cp -a "${PUBLISH_DIR}/." "${APPLICATION_DIR}/"


############################################################
# Linux用ファイルを正規化
#
# Windowsで編集した際に混入する可能性がある
# CRLF改行およびUTF-8 BOMを除去します。
############################################################

echo "Normalizing Linux text files..."

LINUX_FILES=(
    "${POSTINST_FILE}"
    "${PRERM_FILE}"
    "${POSTRM_FILE}"
    "${SERVICE_FILE}"
)

for file in "${LINUX_FILES[@]}"; do
    # CRLFからLFへ変換
    sed -i 's/\r$//' "${file}"

    # ファイル先頭のUTF-8 BOMを除去
    sed -i '1s/^\xEF\xBB\xBF//' "${file}"
done


############################################################
# Debianメンテナンススクリプトへ実行権限を付与
############################################################

echo "Setting maintainer script permissions..."

chmod 0755 "${POSTINST_FILE}"
chmod 0755 "${PRERM_FILE}"
chmod 0755 "${POSTRM_FILE}"

# Unitファイルは実行ファイルではないため0644
chmod 0644 "${SERVICE_FILE}"


############################################################
# Debianパッケージを生成
############################################################

PACKAGE_FILE="${OUTPUT_DIR}/${PACKAGE_NAME}_${PACKAGE_VERSION}_${PACKAGE_ARCHITECTURE}.deb"

echo "Building Debian package..."

dpkg-deb \
    --build \
    --root-owner-group \
    "${PACKAGE_ROOT}" \
    "${PACKAGE_FILE}"


############################################################
# 完了表示
############################################################

echo
echo "Package created:"
echo "  ${PACKAGE_FILE}"
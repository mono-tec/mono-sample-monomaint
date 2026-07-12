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
#   1. プロジェクトファイルからVersionを取得
#   2. DEBIAN/controlを動的生成
#   3. publish成果物をパッケージ領域へコピー
#   4. メンテナンススクリプトへ実行権限を付与
#   5. dpkg-debで.debパッケージを生成
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

# 生成した.debの出力先
OUTPUT_DIR="${SCRIPT_DIR}/output"


############################################################
# パッケージ情報
############################################################

# Debianパッケージ名
PACKAGE_NAME="monomaint-sample"

# DebianパッケージのCPUアーキテクチャ
PACKAGE_ARCHITECTURE="amd64"

# Debianパッケージの分類
PACKAGE_SECTION="web"

# パッケージ管理者
PACKAGE_MAINTAINER="mono-tec"

# 実行に必要な依存パッケージ
PACKAGE_DEPENDS="aspnetcore-runtime-10.0"

# パッケージ説明
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
PACKAGE_VERSION="$(echo "${PACKAGE_VERSION}" | tr -d '\r\n[:space:]')"

if [[ -z "${PACKAGE_VERSION}" ]]; then
    echo "ERROR: Version could not be read from project file."
    exit 1
fi

echo "Package version: ${PACKAGE_VERSION}"


############################################################
# パッケージ作業領域を初期化
############################################################

echo "Cleaning package application directory..."

rm -rf "${APPLICATION_DIR}"

mkdir -p "${APPLICATION_DIR}"
mkdir -p "${DEBIAN_DIR}"
mkdir -p "${OUTPUT_DIR}"


############################################################
# DEBIAN/controlを動的生成
#
# 最終行にも改行が入るよう、ヒアドキュメントで生成します。
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


############################################################
# publish成果物をパッケージへコピー
############################################################

echo "Copying publish output..."

cp -a "${PUBLISH_DIR}/." "${APPLICATION_DIR}/"


############################################################
# Debianメンテナンススクリプトへ実行権限を付与
#
# control以外のファイルを対象にします。
#
# 例:
#   postinst
#   prerm
#   postrm
############################################################

echo "Setting maintainer script permissions..."

find "${DEBIAN_DIR}" \
    -maxdepth 1 \
    -type f \
    ! -name control \
    -exec chmod 0755 {} \;


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
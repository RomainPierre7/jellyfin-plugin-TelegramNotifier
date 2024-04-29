# Makefile

# Define variables
OUTPUT_DEV_DIR = bin
OUTPUT_REL_DIR = publish
OUTPUT_DLL_DIR = plugin

# Default targets
all: plugin

# Target for building (development)
build:
	dotnet build --output $(OUTPUT_DEV_DIR)

# Target for publishing (release)
publish:
	dotnet publish --configuration Release --output $(OUTPUT_REL_DIR)

# Target for cleaning temporary files
clean:
	rm -rf $(OUTPUT_REL_DIR)
	rm -rf $(OUTPUT_DEV_DIR)
	rm -rf $(OUTPUT_DLL_DIR)

# Target for only compiling the DLL plugin file (for users)
plugin:
	dotnet publish --output tmp
	mkdir -p $(OUTPUT_DLL_DIR)
	cp tmp/Jellyfin.Plugin.TelegramNotifier.dll $(OUTPUT_DLL_DIR)
	rm -rf tmp

# Target for only compiling the DLL plugin file (for developers)
dev:
	dotnet build --output tmp
	mkdir -p $(OUTPUT_DLL_DIR)
	cp tmp/Jellyfin.Plugin.TelegramNotifier.dll $(OUTPUT_DLL_DIR)
	rm -rf tmp

# Define phony targets (which are not filenames)
.PHONY: all build publish clean plugin dev

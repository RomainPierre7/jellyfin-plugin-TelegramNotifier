# Makefile

# Define variables
OUTPUT_DEV_DIR = bin
OUTPUT_REL_DIR = publish

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
	rm -f Jellyfin.Plugin.TelegramNotifier.dll

# Target for only building the DLL plugin file (for users)
plugin:
	dotnet build --configuration Release --output tmp
	cp tmp/Jellyfin.Plugin.TelegramNotifier.dll .
	rm -rf tmp

# Define phony targets (which are not filenames)
.PHONY: all build publish clean plugin

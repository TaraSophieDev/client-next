<template>
    <aside>
        <nav class="header">
            <button
                class="item backButton"
                @click="handleBack"
                v-if="canGoBack"
            >
                <span class="mdi mdi-arrow-left"></span>
            </button>
            <div class="brand">
                <SpinShareIcon class="icon" />
                <SpinShareLogo class="logo" />
            </div>
        </nav>

        <nav>
            <router-link
                to="/"
                class="item"
                v-tooltip.right="t('general.sidebar.frontpage')"
            >
                <span class="mdi mdi-view-dashboard-outline"></span>
                <span class="label">{{ t('general.sidebar.frontpage') }}</span>
            </router-link>
            <router-link
                to="/search"
                class="item"
                v-tooltip.right="t('general.sidebar.search')"
            >
                <span class="mdi mdi-magnify"></span>
                <span class="label">{{ t('general.sidebar.search') }}</span>
            </router-link>
            <router-link
                to="/discover/new/0"
                class="item"
                v-tooltip.right="t('general.sidebar.new')"
            >
                <span class="mdi mdi-new-box"></span>
                <span class="label">{{ t('general.sidebar.new') }}</span>
            </router-link>
            <router-link
                to="/discover/updated/0"
                class="item"
                v-tooltip.right="t('general.sidebar.updated')"
            >
                <span class="mdi mdi-update"></span>
                <span class="label">{{ t('general.sidebar.updated') }}</span>
            </router-link>
            <router-link
                to="/discover/hotThisWeek/0"
                class="item"
                v-tooltip.right="t('general.sidebar.trending')"
            >
                <span class="mdi mdi-fire"></span>
                <span class="label">{{ t('general.sidebar.trending') }}</span>
            </router-link>
        </nav>

        <nav>
            <router-link
                to="/library"
                class="item"
                v-tooltip.right="t('general.sidebar.library')"
            >
                <span class="mdi mdi-archive-music-outline"></span>
                <span class="label">{{ t('general.sidebar.library') }}</span>
            </router-link>
            <button
                class="item"
                :class="{ active: downloadQueueActive }"
                v-tooltip.right="t('general.sidebar.downloads')"
                @click="toggleDownloadQueue"
            >
                <span class="mdi mdi-download-box-outline"></span>
                <span class="label">{{ t('general.sidebar.downloads') }}</span>
                <div
                    class="badge"
                    v-if="downloadQueueCount !== 0"
                >
                    {{ downloadQueueCount }}
                </div>
            </button>
            <router-link
                to="/settings"
                class="item"
                v-tooltip.right="t('general.sidebar.settings')"
            >
                <span class="mdi mdi-cog-outline"></span>
                <span class="label">{{ t('general.sidebar.settings') }}</span>
            </router-link>
        </nav>

        <DownloadQueue
            @change-active="
                (state) => {
                    downloadQueueActive = state;
                }
            "
        />
    </aside>
</template>

<script setup>
import { ref, inject, onMounted, computed } from 'vue';
import DownloadQueue from '@/components/DownloadQueue.vue';
import SpinShareIcon from '@/assets/icon.svg?component';
import SpinShareLogo from '@/assets/logo.svg?component';
const emitter = inject('emitter');

import { useI18n } from 'vue-i18n';
import router from '@/router';
const { t } = useI18n();

const downloadQueueCount = ref(0);
const downloadQueueActive = ref(false);

emitter.on('queue-get-count-response', (data) => {
    downloadQueueCount.value = data;
});

onMounted(() => {
    window.external.sendMessage(
        JSON.stringify({
            command: 'queue-get-count',
            data: '',
        }),
    );
});

const toggleDownloadQueue = () => {
    emitter.emit('download-queue-toggle');
};

const handleBack = () => {
    router.go(-1);
};

const canGoBack = computed(() => window.history.state.back !== null);
</script>

<style lang="scss" scoped>
aside {
    border-right: 1px solid rgba(var(--colorBaseText), 0.07);
    background: var(--colorBase);
    display: grid;
    grid-template-rows: auto 1fr auto;
    grid-gap: 15px;
    align-items: center;
    padding: 25px;
    width: 300px;

    @media (max-width: 1200px) {
        justify-content: center;
        padding: 25px 0;
        width: 60px;
    }

    & .header {
        min-height: 45px;
        display: grid;
        gap: 15px;
        grid-template-columns: 1fr auto 1fr;
        grid-template-areas: 'backButton logo empty';
        align-items: center;
        justify-content: center;

        & .backButton {
            grid-area: backButton;
            padding: 0;
            width: 42px;
            height: 42px;
            justify-content: center;
        }

        @media (max-width: 1200px) {
            grid-template-columns: auto;
            grid-template-areas: 'backButton' 'logo';
        }
    }

    & .brand {
        display: flex;
        align-items: center;
        justify-content: center;
        grid-area: logo;

        & .icon {
            width: 24px;
            height: 24px;
            display: none;

            @media (max-width: 1200px) {
                display: block;
            }
        }

        & .logo {
            height: 24px;
            display: block;

            @media (max-width: 1200px) {
                display: none;
            }
        }
    }

    & nav {
        display: flex;
        flex-direction: column;
        gap: 10px;

        @media (max-width: 1200px) {
            align-items: center;
        }

        & .item {
            display: flex;
            border-radius: 100px;
            align-items: center;
            gap: 15px;
            text-decoration: none;
            transition: 0.15s ease-in-out all;
            position: relative;
            padding: 10px 20px;
            font-size: 1rem;
            font-family: var(--fontFamily);
            background: transparent;
            border: 0;
            color: rgb(var(--colorBaseText));

            & .mdi {
                font-size: 22px;
            }

            & .badge {
                background: rgb(var(--colorPrimary));
                color: rgb(var(--colorBaseText));
                font-size: 0.75rem;
                font-weight: bold;
                padding: 3px 5px;
                border-radius: 50px;
                position: absolute;
                bottom: 0;
                right: 0;
            }

            &.router-link-exact-active,
            &.active {
                background: rgba(var(--colorPrimary), 15%);
                color: rgba(var(--colorPrimary), 100%);
            }

            &:not(.router-link-exact-active):not(.active):hover {
                background: rgba(var(--colorBaseText), 0.07);
                cursor: pointer;
            }

            @media (max-width: 1200px) {
                width: 42px;
                height: 42px;
                padding: 0;
                justify-content: center;

                & .label {
                    display: none;
                }
            }
        }
    }
}

.ui-console aside {
    & .header {
        grid-template-columns: auto;
        grid-template-areas: 'logo';

        & .backButton {
            display: none;
        }
    }
}
</style>

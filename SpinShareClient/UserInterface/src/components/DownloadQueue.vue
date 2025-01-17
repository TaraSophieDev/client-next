<template>
    <transition name="queue">
        <section
            class="download-queue-backdrop"
            v-if="isActive"
            @click.self="changeState(false)"
            role="dialog"
            aria-modal="true"
        >
            <section class="download-queue">
                <SpinHeader
                    :label="t('general.downloadQueue.title')"
                    type="small"
                >
                    <SpinButton
                        icon="notification-clear-all"
                        :label="t('general.downloadQueue.clear')"
                        @click="handleClearQueue"
                    />
                </SpinHeader>
                <div
                    class="item"
                    v-for="(item, i) in queue"
                    :class="'state-' + item.State"
                    :key="i"
                >
                    <div
                        class="cover"
                        :style="`background-image: url(${item.Cover})`"
                    ></div>
                    <div class="meta">
                        <div class="title">{{ item.Title }}</div>
                        <div class="artist">
                            {{ item.Artist }} &bull; {{ item.Charter }}
                        </div>
                    </div>
                    <div class="actions">
                        <SpinButton
                            v-if="item.State === STATE_QUEUED"
                            icon="trash-can-outline"
                            color="danger"
                            @click="() => handleRemoveFromQueue(item.ID)"
                        />
                        <div
                            class="loading"
                            v-if="
                                [
                                    STATE_DOWNLOADING,
                                    STATE_EXTRACTING,
                                    STATE_COPYING,
                                    STATE_CACHING,
                                ].includes(item.State)
                            "
                        >
                            <span class="mdi mdi-loading"></span>
                        </div>
                    </div>
                </div>
            </section>
        </section>
    </transition>
</template>

<script setup>
import { ref, inject } from 'vue';
const emitter = inject('emitter');
const emit = defineEmits(['change-active']);

import { useI18n } from 'vue-i18n';
const { t } = useI18n();

const isActive = ref(false);
const queue = ref([]);

const STATE_QUEUED = 0;
const STATE_DOWNLOADING = 1;
const STATE_EXTRACTING = 2;
const STATE_COPYING = 3;
const STATE_CACHING = 4;
const STATE_DONE = 5;

emitter.on('download-queue-toggle', () => {
    changeState(!isActive.value);
});
emitter.on('download-queue-open', () => {
    changeState(true);
});
emitter.on('download-queue-close', () => {
    changeState(false);
});
emitter.on('queue-item-update-response', (downloadItem) => {
    let existingItemIndex = queue.value.findIndex(
        (x) => x.ID === downloadItem.ID,
    );

    if (existingItemIndex !== -1) {
        queue.value[existingItemIndex] = downloadItem;
    } else {
        queue.value.push(downloadItem);
    }
});
emitter.on('queue-get-response', (newQueue) => {
    queue.value = newQueue;
});

const changeState = (newState) => {
    isActive.value = newState;
    emit('change-active', isActive.value);

    if (newState) {
        window.external.sendMessage(
            JSON.stringify({
                command: 'queue-get',
                data: '',
            }),
        );
    }
};

const handleClearQueue = () => {
    window.external.sendMessage(
        JSON.stringify({
            command: 'queue-clear',
            data: STATE_DONE,
        }),
    );
};

const handleRemoveFromQueue = (queueId) => {
    window.external.sendMessage(
        JSON.stringify({
            command: 'queue-remove',
            data: queueId,
        }),
    );
};
</script>

<style lang="scss">
.download-queue-backdrop {
    position: absolute;
    left: 300px;
    bottom: 0;
    top: 0;
    right: 0;
    background: rgba(0, 0, 0, 0.6);
    backdrop-filter: blur(5px);
    z-index: 100;
    cursor: pointer;

    @media (max-width: 1200px) {
        left: 60px;
    }

    & .download-queue {
        cursor: default;
        width: 400px;
        height: 100%;
        overflow-y: scroll;
        background: rgb(var(--colorBase));
        border-right: 1px solid rgba(var(--colorBaseText), 0.07);
        display: flex;
        flex-direction: column;

        & header {
            padding: 10px 20px;
        }

        & .item {
            display: grid;
            grid-template-columns: auto 1fr auto;
            align-items: center;
            gap: 10px;
            padding: 15px;
            border-bottom: 1px solid rgba(var(--colorBaseText), 0.07);

            & .cover {
                width: 48px;
                height: 48px;
                border-radius: 4px;
                background-position: center;
                background-size: cover;
            }
            & .meta {
                display: grid;
                gap: 3px;

                & .artist {
                    color: rgba(var(--colorBaseText), 0.4);
                    font-size: 0.9rem;
                }
            }
            & .actions {
                & .loading {
                    width: 28px;
                    height: 28px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 22px;
                    animation: loadingLoop 0.4s linear infinite;
                }
            }

            &.state-0 {
                opacity: 0.6;
            }
            &.state-5 {
                background: rgba(var(--colorSuccess), 0.14);
            }
        }
    }
}

.ui-console .download-queue-backdrop {
    left: 400px;

    & .download-queue {
        width: 600px;
    }
}
</style>

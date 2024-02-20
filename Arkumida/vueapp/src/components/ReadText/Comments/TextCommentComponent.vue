<script setup>
    import {defineProps} from "vue";
    import moment from "moment";
    import {AvatarClass, Messages} from "@/js/constants";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";

    const props = defineProps({
        comment: Object
    })

    const authorLink =
    {
        href: "/users/" + props.comment.author.entityId,
        name: props.comment.author.name,
        title: Messages.CreatureUser + props.comment.author.name
    }
</script>

<template>
    <div class="read-text-comments-comment-container">

        <!-- Author's avatar -->
        <div>
            <a
                class="read-text-comments-comment-author-link"
                :href="authorLink.href"
                :title="authorLink.title">
                <AvatarComponent :avatar="props.comment.author.currentAvatar" :avatarClass="AvatarClass.Small" />
            </a>
        </div>

        <div class="vertical-flex">

            <!-- Author and post / edit times -->
            <div class="read-text-comments-info-line">

                <!-- Author -->
                <div class="read-text-comments-comment-author-name">
                    <a
                        class="read-text-comments-comment-author-link"
                        :href="authorLink.href"
                        :title="authorLink.title">{{ authorLink.name }}</a>
                </div>

                <!-- Post time (always visible) -->
                <div class="read-text-comments-comment-dates">
                    {{ moment(props.comment.postTime).format('HH:mm DD.MM.YYYY')  }}
                </div>
            </div>

            <!-- Message itself -->
            <div>
                {{ props.comment.message }}
            </div>

        </div>

    </div>
</template>
